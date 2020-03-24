(defblock :name convert-metadata :is-top t
	(worker
		:worker-name convert-metadata
		:verb "cm"
		:description "Convert DB metadata stored as a JSON file"
		:usage-samples (
			"tau db cm 'c:/temp/dbmetadata.json' --target-provider sqlite"
			"tau db cm 'c:/temp/dbmetadata.json' -tp postgresql"
		)
	)

	(some-text
		:classes path string term ; term: e.g. stdin
		:alias source-file
		:action argument
		:is-mandatory t
		:description "File or stream to read metadata from"
		:doc-subst "source file")

	(multi-text
		:classes key
		:values "-tp" "--target-provider"
		:alias target-provider
		:action key
		:is-mandatory t)
	(multi-text
		:classes term
		:values "sqlserver" "postgresql" "mysql" "sqlite"
		:action value
		:description "Target DB provider identifier"
		:doc-subst "target db provider")

	(opt
		(multi-text
			:classes key
			:values "-v" "--verbose"
			:alias verbose
			:action option
			:description "Verbose output")
	)

	(end)
)
