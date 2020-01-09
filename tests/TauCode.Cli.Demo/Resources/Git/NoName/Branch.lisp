(defblock :name branch :is-top t
	(worker
		:worker-name branch
		:verbs "branch"
		:doc "Branch management."
		:usage-samples (
			"<todo>"
			"<todo>"
			))
		:properties (
			(doc "Branch management")
			(usage-samples "usage1\r\nusage2")
		)
	(idle :name args)
	(alt
		(key-value-pair
			:alias connection
			:key-names "--conn" "-c"
			:key-values (choice :classes string path :values *)

			; zeta below
			:keys "--conn" "-c"
			:value-classes path
			:values *
			:properties (
				("alias" "connection")
				("doc" "Connection string")
			))

		(key-value-pair
			:alias provider
			:key-names "--provider" "-p"
			:key-values (choice :classes term :values "sqlserver" "postgresql"))

		(key-value-pair
			:alias exclude
			:key-names "--exclude" "-e"
			:key-values (choice :classes string term :values *))

	)
	(idle :links args next)
	(end)
)
