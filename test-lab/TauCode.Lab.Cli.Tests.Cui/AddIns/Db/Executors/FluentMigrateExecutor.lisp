(defblock :name fluent-migrate :is-top t
	(executor
		;:executor-name fluent-migrate
		;:verb "fluent-migrate"
		:description "Migrate database"
		:usage-samples (
			"db fluent-migrate -p sqlserver -c Server=.;Database=mydb;Trusted_Connection=True; -r"
		)
	)

	(idle :name args)

	(alt
		(seq
			(multi-text
				:classes key
				:values "-p" "--provider"
				:alias provider
				:action key
				:is-mandatory t)
			(multi-text
				:classes term
				:values "sqlserver" "postgresql" "mysql"
				:action value
				:description "DB provider identifier"
				:doc-subst "db provider"))

		(seq
			(multi-text
				:classes key
				:values "-c" "--connection"
				:alias connection
				:action key
				:is-mandatory t)
			(some-text
				:classes path string
				:action value
				:description "DB connection string to use"
				:doc-subst "connection string"))

		(seq
			(multi-text
				:classes key
				:values "-s" "--schema"
				:alias schema
				:action key)
			(some-text
				:classes path string
				:action value
				:description "Schema name"
				:doc-subst "schema name"))
	)


	(idle :links args next)

	(idle :name options)

	(opt
		(alt
			(multi-text
				:classes key
				:values "-r" "--reset"
				:alias reset
				:action option
				:description "Reset DB")

			(multi-text
				:classes key
				:values "-v" "--verbose"
				:alias verbose
				:action option
				:description "Verbose output")
		)
	)


	(idle :links options next)

	(end)
)
