(defblock :name migrate :is-top t
	(worker
		:worker-name migrate
		:verb "migrate"
		:description "Migrate database"
		:usage-samples (
			"tau db migrate Server=.;Database=mydb;Trusted_Connection=True; -p sqlserver -a c:\temp\my-migrations.dll -r"
		)
	)

	(some-text
		:classes path string
		:alias connection-string
		:action argument		
		:description "DB connection string to use"
		:doc-subst "connection string")

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
				:values "-a" "--assembly"
				:alias migration-assembly
				:action key)
			(some-text
				:classes path string
				:alias migration-assembly-path
				:action value
				:description "Migration assembly path"
				:doc-subst "migration assembly path"))
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
